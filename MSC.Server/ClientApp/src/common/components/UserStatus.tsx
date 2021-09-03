import { Box, Button, Center, HStack } from '@chakra-ui/react';
import React, { FC } from 'react';

export const UserStatus: FC = () => {
  return <Box p="6px">
    <Center>User</Center>
    <HStack>
      <Button>注销</Button>
    </HStack>
  </Box>
};
