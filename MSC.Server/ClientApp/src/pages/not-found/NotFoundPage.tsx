import { Center, Heading } from '@chakra-ui/react';
import React, { FC } from 'react';

export const NotFoundPage: FC = () => {
  return (
    <Center minHeight="100vh">
      <Heading size="3xl">Oh，看起来你迷路了</Heading>
      <Center>这里可不是你该来的地方</Center>
    </Center>
  );
};
