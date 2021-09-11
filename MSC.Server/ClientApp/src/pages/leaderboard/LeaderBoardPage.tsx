import {
  Center,
  Container,
  Heading,
  Table,
  Tbody,
  Td,
  Th,
  Thead,
  Tr,
  Text
} from '@chakra-ui/react';
import React, { FC, useEffect, useMemo } from 'react';
import { LoadingMask } from '../../common/components/LoadingMask';
import { INFO_API } from '../../redux/info.api';
import ReactEChartsCore from 'echarts-for-react/lib/core';
import * as echarts from 'echarts/core';
import { LineChart } from 'echarts/charts';
import { GridComponent, TooltipComponent, TitleComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';

function formatTime(time: string) {
  const date = new Date(time);
  return `${date.getMonth() + 1}/${date.getDate()} ${date.getHours()}:${date.getMinutes()}`;
}

export const LeaderBoardPage: FC = () => {
  const { isLoading, error, data } = INFO_API.useGetScoreBoardQuery();

  const chartData = useMemo(() => {
    if (!data) {
      return { xAxisData: [], seriesData: [] };
    }
    const xAxisDataRaw = [
      ...new Set(
        data.topDetail
          .map((item) => item.timeLine)
          .flat()
          .map((item) => item.time)
      )
    ].sort();
    const xAxisData = xAxisDataRaw
      .map((time) => new Date(time).getTime())
      .map((time) => ((time - new Date(xAxisDataRaw[0]).getTime()) / 1000 / 60 / 60).toFixed(2)); // to hours
    const seriesData = data.topDetail.map((item) => {
      return {
        type: 'line',
        connectNulls: true,
        name: item.userName,
        data: xAxisDataRaw.map((time) => item.timeLine.find((t) => t.time === time)?.score ?? null)
      };
    });
    return {
      xAxisData,
      seriesData
    };
  }, [data]);

  useEffect(() => {
    echarts.use([TitleComponent, TooltipComponent, GridComponent, LineChart, CanvasRenderer]);
  }, []);

  if (isLoading || error) {
    return <LoadingMask error={error} />;
  }

  if (data!.rank.length <= 0) {
    return <Center>暂无数据</Center>;
  }

  return (
    <Container minH="100vh" p="24px" maxWidth="80ch">
      <Center mb="6px">
        <Heading size="md">前十名分数变化图</Heading>
      </Center>
      <ReactEChartsCore
        echarts={echarts}
        style={{
          height: 400
        }}
        option={{
          xAxis: {
            type: 'category',
            name: '距开始\n时间',
            axisLabel: {
              formatter: '{value} 小时'
            },
            data: chartData.xAxisData
          },
          yAxis: {
            type: 'value',
            name: '分数',
            axisLabel: {
              formatter: '{value} 分'
            },
            splitLine: {
              show: false
            }
          },
          tooltip: {
            trigger: 'axis',
            textStyle: {
              fontSize: 10,
              color: '#ffffffeb'
            },
            backgroundColor: '#414141'
          },
          series: chartData.seriesData
        }}
      />
      <Center mb="24px">
        <Heading size="md">排行榜</Heading>
      </Center>
      <Table w="100%" bg="gray.800" mx="auto">
        <Thead>
          <Tr>
            <Th>
              <Text textAlign="left">名称</Text>
            </Th>
            <Th>
              <Text textAlign="left">介绍</Text>
            </Th>
            <Th>
              <Text textAlign="right">分数</Text>
            </Th>
            <Th>
              <Text textAlign="right">更新时间</Text>
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          {data?.rank.map((item) => (
            <Tr key={item.name + item.descr} fontSize="sm">
              <Td>
                <Text>{item.name}</Text>
              </Td>
              <Td p="2px">
                <Text maxWidth="24em" isTruncated>
                  {item.descr}
                </Text>
              </Td>
              <Td isNumeric>
                <Text fontFamily="mono">{item.score}</Text>
              </Td>
              <Td isNumeric>
                <Text fontFamily="mono">{formatTime(item.time)}</Text>
              </Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </Container>
  );
};
