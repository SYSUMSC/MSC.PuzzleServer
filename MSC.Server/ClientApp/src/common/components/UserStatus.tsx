import { Text, Box, Button, Center, Heading, HStack, Spinner, VStack } from '@chakra-ui/react';
import { Link } from 'react-router-dom';
import React, { FC } from 'react';
import { Redirect } from 'react-router';
import { USER_API } from 'src/redux/user.api';

export const UserStatus: FC = () => {
  const { data: user } = USER_API.useStatusQuery();
  const [logout, { isLoading: isLoggingOut, isSuccess: isLogOutSuccess }] =
    USER_API.useLogoutMutation();

  if (isLogOutSuccess) {
    return <Redirect to="/login" />;
  }

  return (
    <Box py="6px" px="12px">
      {!user && (
        <Center>
          <Spinner />
        </Center>
      )}
      {user && (
        <VStack spacing="12px">
          <Heading size="md">{user.name}</Heading>
          <Text>{user.email}</Text>
          <HStack>
            <Link to="/change-email">
              <Button size="sm" variant="ghost">
                修改邮箱
              </Button>
            </Link>
            <Button
              size="sm"
              variant="ghost"
              disabled={isLoggingOut}
              isLoading={isLoggingOut}
              onClick={() => logout()}
            >
              注销
            </Button>
          </HStack>
        </VStack>
      )}
    </Box>
  );
};
