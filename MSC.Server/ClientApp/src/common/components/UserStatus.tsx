import { Text, Box, Button, Center, Heading, HStack, Spinner, VStack } from '@chakra-ui/react';
import React, { FC } from 'react';
import { Redirect } from 'react-router';
import { USER_API } from 'src/redux/user.api';

export const UserStatus: FC = () => {
  const { data: user } = USER_API.useStatusQuery();
  const [logout, { isLoading, isSuccess }] = USER_API.useLogoutMutation();

  if (isSuccess) {
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
            <Button
              size="sm"
              variant="outline"
              disabled={isLoading}
              isLoading={isLoading}
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
