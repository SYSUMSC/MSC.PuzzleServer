import {
  Box,
  Container,
  Text,
  VStack,
  Wrap,
  WrapItem,
  Heading,
  Flex,
  Center,
  HStack
} from '@chakra-ui/react';
import React, { FC, useMemo } from 'react';
import { Link } from 'react-router-dom';
import { LoadingMask } from 'src/common/components/LoadingMask';
import { PUZZLE_API } from 'src/redux/puzzle.api';

interface PuzzleCardProps {
  id: number;
  isSolved: boolean;
  acceptedCount: number;
  submissionCount: number;
  title: string;
}

const PuzzleCard: FC<PuzzleCardProps> = ({
  id,
  isSolved,
  acceptedCount,
  submissionCount,
  title
}) => (
  <Link to={`puzzle/${id}`}>
    <Box
      rounded="lg"
      bg="gray.700"
      overflow="hidden"
      w="220px"
      shadow="xl"
      _hover={{ background: 'gray.600' }}
      transition="background 0.2s ease"
    >
      <Box w="100%" h="4px" bg={isSolved ? 'green.400' : 'gray.400'} />
      <VStack px="18px" py="12px" align="sketch" spacing="0">
        <Text isTruncated noOfLines={1} fontWeight="bold" mb="24px">
          {title}
        </Text>
        <Flex justifyContent="space-between" alignItems="flex-end">
          <HStack spacing="12px">
            <VStack spacing="0" align="sketch">
              <Text color="gray.400" fontSize="xs" textAlign="right">
                提交次数
              </Text>
              <Text fontFamily="mono" textAlign="right">
                {submissionCount}
              </Text>
            </VStack>
            <VStack spacing="0" align="sketch">
              <Text color="gray.400" fontSize="xs" textAlign="right">
                回答正确率
              </Text>
              <Text fontFamily="mono" textAlign="right">
                {((acceptedCount / submissionCount) * 100).toFixed(2) || 0}%
              </Text>
            </VStack>
          </HStack>
          <Text color="gray.400" fontStyle="italic">
            #{id}
          </Text>
        </Flex>
      </VStack>
    </Box>
  </Link>
);

export const PuzzlePage: FC = () => {
  const { isLoading, data, error } = PUZZLE_API.useGetPuzzleListQuery();

  const props = useMemo<PuzzleCardProps[]>(() => {
    if (!data) {
      return [];
    }
    return data.accessible.map((item) => ({
      id: item.id,
      title: item.title,
      acceptedCount: item.acceptedCount,
      submissionCount: item.submissionCount,
      isSolved: data.solved.includes(item.id)
    }));
  }, [data]);

  if (error || isLoading) {
    return <LoadingMask error={error} />;
  }

  return (
    <Container maxWidth="80ch" minH="100vh" py="48px">
      <Center mb="24px">
        <Heading size="md">你的谜题</Heading>
      </Center>
      <Wrap spacing="48px">
        {props.map((p, index) => (
          <WrapItem key={p.id}>
            <PuzzleCard {...p} />
          </WrapItem>
        ))}
      </Wrap>
    </Container>
  );
};
