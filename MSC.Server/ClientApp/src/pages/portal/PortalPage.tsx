import React, { FC } from 'react';
import { LoadingMask } from 'src/common/components/LoadingMask';
import {
  Box,
  Center,
  Container,
  Heading,
  VStack,
  Wrap,
  WrapItem,
  Text,
  Flex
} from '@chakra-ui/react';
import { INFO_API, Announcement } from '../../redux/info.api';
import marked from 'marked';

function formatTime(time: string) {
  const date = new Date(time);
  return `${date.getMonth() + 1}/${date.getDate()} ${date.getHours()}:${date.getMinutes()}`;
}

const AnnouncementCard: FC<Announcement> = ({ time, isPinned, content, title }) => (
  <Box
    rounded="lg"
    bg="gray.700"
    overflow="hidden"
    w="100%"
    shadow="xl"
    _hover={{ background: 'gray.600' }}
    transition="background 0.2s ease"
  >
    <Flex justifyContent="space-between" alignItems="flex-end">
      <VStack px="18px" py="12px" align="sketch" spacing="0">
        <Heading color="gray.300" size="2xl" textShadow="xl">
          # {title}
        </Heading>
        <Box
          flex="1"
          my="24px"
          overflow="auto"
          dangerouslySetInnerHTML={{ __html: marked(content) }}
        />
        <Text fontFamily="mono" textAlign="right" color="gray.400">
          # {formatTime(time)}
        </Text>
      </VStack>
    </Flex>
  </Box>
);

export const PortalPage: FC = () => {
  const { isLoading, error, data } = INFO_API.useGetAnnouncementsQuery();

  if (error || isLoading) {
    return <LoadingMask error={error} />;
  }

  return (
    <Container minHeight="100vh">
      <Center h="100vh">
        <Box>
          <Heading textShadow="2xl">欢迎来到 SYSUMSC 解谜游戏</Heading>
        </Box>
      </Center>
      <Wrap spacing="45px" justify="center">
        {data?.map((a) => (
          <WrapItem key={a.title + a.time}>
            <AnnouncementCard {...a} />
          </WrapItem>
        ))}
      </Wrap>
    </Container>
  );
};
