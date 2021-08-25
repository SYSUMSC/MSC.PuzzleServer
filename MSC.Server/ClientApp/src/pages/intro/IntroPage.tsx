import React, { FC } from 'react';
import { Center, Heading, VStack, Text } from '@chakra-ui/react';

export const IntroPage: FC = () => {
  return (
    <Center minHeight="100vh">
      <VStack>
        <Heading size="3xl" mb="6rem">
          Title here
        </Heading>
        <Text>Some Text with a hiden link</Text>
      </VStack>
    </Center>
  );
};
