import { Container } from '@chakra-ui/react';
import React, { FC } from 'react';
import { Route } from 'react-router';
import { IntroPage } from './pages/intro/IntroPage';
import { LoginPage } from './pages/login/LoginPage';
import { NotFoundPage } from './pages/not-found/NotFoundPage';

export const App: FC = () => {
  return (
    <Container minHeight="100vh" width="100vw">
      <Route exact path="/">
        <IntroPage />
      </Route>
      <Route path="/login">
        <LoginPage />
      </Route>
      <Route path="*">
        <NotFoundPage />
      </Route>
    </Container>
  );
};
