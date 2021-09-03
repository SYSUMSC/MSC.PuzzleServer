import { Box, Flex } from '@chakra-ui/react';
import React, { FC } from 'react';
import { NavBar } from './NavBar';

export const WithNavBar: FC = ({ children }) => {
  return (
    <Flex minW="100vw">
      <Box minH="100vh" flex="none" mx="24px" boxShadow="2xl">
        <NavBar />
      </Box>
      <Box minH="100vh" flex="1">
        {children}
      </Box>
    </Flex>
  );
};
