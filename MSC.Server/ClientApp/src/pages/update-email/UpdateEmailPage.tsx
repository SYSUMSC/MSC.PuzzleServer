import React, { FC, FormEvent, useCallback, useEffect, useState } from 'react';
import {
  Button,
  Container,
  Flex,
  FormControl,
  FormErrorMessage,
  FormLabel,
  Heading,
  Input,
  useToast
} from '@chakra-ui/react';
import { resolveMessage } from '../../common/utils';
import { USER_API } from '../../redux/user.api';

export const UpdateEmailPage: FC = () => {
  const [changeEmail, { isLoading, error, isSuccess }] = USER_API.useChangeEmailMutation();
  const [email, setEmail] = useState('');
  const toast = useToast();

  useEffect(() => {
    if (isSuccess) {
      toast({
        title: '提交成功',
        description: '验证邮件已经发往指定的邮箱，请跟随邮箱内容指示操作',
        status: 'success',
        duration: 10000
      });
    }
  }, [isSuccess, toast]);

  const onSubmit = useCallback(
    (event: FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      if (email) {
        changeEmail({ newMail: email });
      }
    },
    [email, changeEmail]
  );

  return (
    <Container p="32px" maxWidth="40ch">
      <Heading mb="32px" size="lg">
        修改邮箱
      </Heading>
      <form onSubmit={onSubmit}>
        <FormControl id="email" my="12px" isRequired isInvalid={!!error}>
          <FormLabel>邮箱</FormLabel>
          <Input type="email" value={email} onChange={(event) => setEmail(event.target.value)} />
          {error && <FormErrorMessage>{resolveMessage(error)}</FormErrorMessage>}
        </FormControl>
        <Flex mt="32px" direction="row-reverse">
          <Button type="submit" isLoading={isLoading} disabled={!email || isLoading || isSuccess}>
            提交
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
