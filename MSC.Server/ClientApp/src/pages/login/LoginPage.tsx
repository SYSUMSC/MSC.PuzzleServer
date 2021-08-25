import {
  Box,
  Button,
  Center,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Input,
  Spacer,
  useBoolean,
  VStack
} from '@chakra-ui/react';
import { or } from '../../utils';
import React, { FC, FormEvent } from 'react';

export const LoginPage: FC = () => {
  const [isToLogin, { toggle: toggleIsToLogin }] = useBoolean(true);

  const onLogin = React.useCallback((event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  }, []);

  const onRegister = React.useCallback((event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  }, []);

  return (
    <Center minHeight="100vh">
      <Box boxShadow="2xl" bg="gray.700" rounded="lg" px="48px" py="24px">
        <VStack mx="24px">
          <Center mb="24px">
            <Heading>Nice LOGO</Heading>
          </Center>
          <Center>
            <Heading
              size="lg"
              cursor="pointer"
              color={or(!isToLogin, 'gray.500')}
              onClick={toggleIsToLogin}
            >
              登陆
            </Heading>
            <Heading
              size="lg"
              pl="12px"
              cursor="pointer"
              color={or(isToLogin, 'gray.500')}
              onClick={toggleIsToLogin}
            >
              注册
            </Heading>
          </Center>
          {!isToLogin && (
            <form onSubmit={onRegister}>
              <FormControl id="username" my="12px" isRequired>
                <FormLabel>用户名</FormLabel>
                <Input type="text" />
              </FormControl>
              <FormControl id="email" my="12px" isRequired>
                <FormLabel>邮箱</FormLabel>
                <Input type="email" />
              </FormControl>
              <FormControl id="password" my="12px" isRequired>
                <FormLabel>密码</FormLabel>
                <Input type="password" />
              </FormControl>
              <FormControl id="confirm-password" my="12px" isRequired>
                <FormLabel>确认密码</FormLabel>
                <Input type="password" />
              </FormControl>
              <Flex mt="24px">
                <Spacer />
                <Button type="submit" variant="outline">
                  注册
                </Button>
              </Flex>
            </form>
          )}
          {isToLogin && (
            <form onSubmit={onLogin}>
              <FormControl id="username" my="12px">
                <FormLabel>用户名</FormLabel>
                <Input type="text" />
              </FormControl>
              <FormControl id="password" my="12px">
                <FormLabel>密码</FormLabel>
                <Input type="password" />
              </FormControl>
              <Flex mt="24px">
                <Button variant="link">忘记密码</Button>
                <Spacer />
                <Button type="submit" variant="outline">
                  登陆
                </Button>
              </Flex>
            </form>
          )}
        </VStack>
      </Box>
    </Center>
  );
};
