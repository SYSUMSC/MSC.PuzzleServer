import {
  Text,
  Box,
  Button,
  Center,
  Container,
  Flex,
  Heading,
  Input,
  useToast
} from '@chakra-ui/react';
import React, { FC, FormEvent, useCallback, useState } from 'react';
import { LoadingMask } from 'src/common/components/LoadingMask';
import { resolveMessage } from 'src/common/utils';
import { PUZZLE_API } from 'src/redux/puzzle.api';
import marked from 'marked';

export interface PuzzleDetailPageProps {
  id: number;
}

export const PuzzleDetailPage: FC<PuzzleDetailPageProps> = ({ id }) => {
  const { isLoading, error, data } = PUZZLE_API.useGetPuzzleQuery(id, { skip: Object.is(NaN, id) });
  const [submit, { isLoading: isAnswering, error: answerError, isSuccess: isAnswerSuccess }] =
    PUZZLE_API.useAnswerPuzzleMutation();
  const [answer, setAnswer] = useState('');
  const toast = useToast();

  const onSubmit = useCallback(
    (event: FormEvent) => {
      event.preventDefault();
      if (answer) {
        submit([{ answer }, id]);
      }
    },
    [answer, submit, id]
  );

  if (isAnswerSuccess) {
    toast({
      title: '回答正确',
      description: '恭喜你找到了这道谜题的答案！',
      status: 'success',
      duration: 5000
    });
  }

  if (Object.is(NaN, id)) {
    return (
      <Center h="100%">
        <Text>无效的谜题 ID</Text>
      </Center>
    );
  }

  if (error || isLoading) {
    return <LoadingMask error={error} />;
  }

  return (
    <Container display="flex" flexDirection="column" height="100vh">
      <Flex flex="none" alignItems="center" mt="10vh">
        <Heading color="gray.300" size="2xl" textShadow="xl">
          # {data!.title}
        </Heading>
      </Flex>
      <Box
        flex="1"
        my="24px"
        overflow="auto"
        dangerouslySetInnerHTML={{ __html: marked(data!.content) }}
      />
      <Box flex="none" p="12px" roundedTopLeft="xl" roundedTopRight="xl" bg="gray.700">
        <Flex as="form" onSubmit={onSubmit}>
          <Input
            flex="1"
            mr="8px"
            placeholder="输入你的答案……"
            value={answer}
            onChange={(event) => setAnswer(event.target.value)}
          />
          <Button
            flex="none"
            type="submit"
            disabled={!answer || isAnswering}
            isLoading={isAnswering}
          >
            {answerError && resolveMessage(answerError)}
            {!answerError && '提交'}
          </Button>
        </Flex>
      </Box>
    </Container>
  );
};
