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
  HStack,
  keyframes
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
      w="185px"
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
          <HStack spacing="5px">
            <VStack spacing="0" align="sketch">
              <Text color="gray.400" fontSize="xs" textAlign="right">
                正确率
              </Text>
              <Text fontFamily="mono" textAlign="right">
                {((acceptedCount / submissionCount) * 100).toFixed(1) || 0}%
              </Text>
            </VStack>
            <VStack spacing="0" align="sketch">
              <Text color="gray.400" fontSize="xs" textAlign="right">
                &emsp;提交
              </Text>
              <Text fontFamily="mono" textAlign="right">
                {submissionCount}
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
  const fadeAnimation = `${keyframes`
  0% {opacity:0;}
  100% {opacity:1;}
  `} 0.5s linear`;

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
      <Wrap spacing="45px" justify="center">
        {props.map((p) => (
          <WrapItem key={p.id} animation={fadeAnimation}>
            <PuzzleCard {...p} />
          </WrapItem>
        ))}
      </Wrap>
    </Container>
  );
};
