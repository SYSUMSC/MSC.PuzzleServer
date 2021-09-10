import React, { FC, useEffect } from 'react';
import { USER_API } from '../../redux/user.api';
import { Redirect } from 'react-router';
import { useQueryParams } from '../../common/hooks/use-query-params';
import { LoadingMask } from '../../common/components/LoadingMask';
import { Box, Center } from '@chakra-ui/react';

export const VerifyEmailPage: FC = () => {
  const [verify, { isLoading, error, isSuccess }] = USER_API.useVerifyEmailMutation();
  const { token, email } = useQueryParams();

  useEffect(() => {
    if (token && email) {
      verify({
        token,
        email
      });
    }
  }, [token, email, verify]);

  useEffect(() => {
    if (isSuccess) {
      setTimeout(() => {
        window.location.href = '/login';
      }, 3000);
    }
  }, [isSuccess]);

  if (!token || !email) {
    return <Redirect to="/" />;
  }

  if (isLoading) {
    return (
      <Box h="100vh" w="100vw">
        <LoadingMask message="正在验证邮箱" />
      </Box>
    );
  }

  if (error) {
    return (
      <Box h="100vh" w="100vw">
        <LoadingMask error={error} />
      </Box>
    );
  }

  return (
    <Box h="100vh" w="100vw">
      <Center>验证成功，即将跳转至登陆页</Center>
    </Box>
  );
};
