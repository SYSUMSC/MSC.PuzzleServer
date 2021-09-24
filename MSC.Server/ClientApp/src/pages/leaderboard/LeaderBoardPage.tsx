import {
  Center,
  Container,
  Heading,
  HStack,
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
import { SYSUIcon } from '../../common/components/SYSUIcon';
import { LegendComponent, DataZoomComponent } from 'echarts/components';
import { GridComponent, TooltipComponent, TitleComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';

function formatNumber(num: number) {
  return num < 10 ? '0' + num : num;
}

function formatTime(time: string) {
  const date = new Date(time);
  return `${formatNumber(date.getMonth() + 1)}/${formatNumber(date.getDate())} ${formatNumber(date.getHours())}:${formatNumber(date.getMinutes())}`;
}

export const LeaderBoardPage: FC = () => {
  const { isLoading, error, data } = INFO_API.useGetScoreBoardQuery();

  const chartData = useMemo(
    () =>
      data?.topDetail.map((item) => ({
        type: 'line',
        step: 'end',
        name: item.userName,
        data: item.timeLine.map((item) => [item.time, item.score])
      })),
    [data]
  );

  useEffect(() => {
    echarts.use([
      TitleComponent,
      TooltipComponent,
      GridComponent,
      LineChart,
      CanvasRenderer,
      LegendComponent,
      DataZoomComponent
    ]);
  }, []);

  if (isLoading || error) {
    return <LoadingMask error={error} />;
  }

  if (data!.rank.length <= 0) {
    return <LoadingMask message="暂无数据" />;
  }

  return (
    <Container minH="100vh" p="24px" maxWidth="80ch">
      <Center mb="5px">
        <Heading size="md">前十名分数变化</Heading>
      </Center>
      <ReactEChartsCore
        echarts={echarts}
        style={{
          height: 460
        }}
        option={{
          xAxis: {
            type: 'time',
            name: '时间',
            splitLine: {
              show: false
            }
          },
          yAxis: {
            type: 'value',
            name: '分数',
            boundaryGap: [0, '100%'],
            axisLabel: {
              formatter: '{value} 分'
            },
            splitLine: {
              show: true,
              lineStyle: {
                color: ['#505050'],
                type: 'dashed'
              }
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
          legend: {
            orient: 'vertical',
            bottom: 60,
            height: '40px',
            textStyle: {
              fontSize: 10,
              color: '#ffffffeb'
            }
          },
          grid: {
            y: 50,
            y2: 140,
          },
          dataZoom: [
            {
              type: 'inside',
              start: 0,
              end: 100,
              filterMode: 'none'
            },
            {
              start: 0,
              end: 100,
              showDetail: false
            }
          ],
          series: chartData
        }}
      />
      <Center m="24px">
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
                <HStack>
                  {item.isSYSU && (
                    <Center h="18px" w="18px" color="brand.100">
                      <SYSUIcon width="18px" height="18px"/>
                    </Center>
                  )}
                  <Text>{item.name}</Text>
                </HStack>
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
