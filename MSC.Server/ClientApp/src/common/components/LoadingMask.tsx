import React, { FC } from 'react';
import { VStack, Center, Spinner } from '@chakra-ui/react';

export const LoadingMask: FC = () => {
  return (
    <Center w="100vw" h="100vh">
      <VStack spacing={4}>
        <Spinner thickness="4px" size="xl" color="brand.100" />
      </VStack>
    </Center>
  );
};
