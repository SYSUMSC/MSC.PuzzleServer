import {
  Box,
  Button,
  Container,
  Flex,
  Heading,
  Input
} from '@chakra-ui/react';
import React, { FC } from 'react';

export interface PuzzleDetailPageProps {
  id?: string;
}

export const PuzzleDetailPage: FC<PuzzleDetailPageProps> = ({ id }) => {
  // check 404

  return (
    <Container display="flex" flexDirection="column" height="100vh">
      <Flex flex="none" alignItems="center" mt="10vh">
        <Heading color="gray.300" size="4xl" textShadow="xl"># Title</Heading>
      </Flex>
      <Box flex="1" my="24px" overflow="auto">adjiawiduhjawiudashduis</Box>
      <Box flex="none" p="12px" roundedTopLeft="xl" roundedTopRight="xl" bg="gray.700">
        <Flex>
          <Input flex="1" mr="8px" placeholder="输入你的答案……" />
          <Button flex="none">提交</Button>
        </Flex>
      </Box>
    </Container>
  );
};
