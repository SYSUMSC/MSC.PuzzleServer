import React, { FC } from 'react';
import { Box, Center, Container, Heading } from '@chakra-ui/react';

export const PortalPage: FC = () => {
  return (
    <Container minHeight="100vh">
      <Center h="100vh">
        <Box>
          <Heading textShadow="2xl">欢迎来到 SYSUMSC 解谜游戏</Heading>
        </Box>
      </Center>
    </Container>
  );
};
