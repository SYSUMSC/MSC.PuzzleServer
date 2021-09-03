import React, { FC } from 'react';
import { VStack, Center, Spinner } from '@chakra-ui/react';

export const LoadingMask: FC = () => {
  return (
    <Center w="100vw" h="100vh">
      <VStack spacing={4}>
        <Spinner thickness="4px" speed="0.65s" size="xl" />
      </VStack>
    </Center>
  );
};
