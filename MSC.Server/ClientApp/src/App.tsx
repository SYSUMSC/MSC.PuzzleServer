import { Box } from '@chakra-ui/react';
import React, { FC } from 'react';
import { Redirect, Route, Switch } from 'react-router';
import { AuthRoute } from './common/routes/AuthRoute';
import { LoginPage } from './pages/login/LoginPage';
import { NotFoundPage } from './pages/not-found/NotFoundPage';
import { WithNavBar } from './common/components/WithNavBar';
import { PortalPage } from './pages/portal/PortalPage';
import { PuzzlePage } from './pages/puzzle/PuzzlePage';
import { LeaderBoardPage } from './pages/leaderboard/LeaderBoardPage';
import { PuzzleDetailPage } from './pages/puzzle-detail/PuzzleDetailPage';
import { ResetPasswordPage } from './pages/reset/ResetPasswordPage';
import { VerifyEmailPage } from './pages/verify/VerifyEmailPage';
import { UpdateEmailPage } from './pages/update-email/UpdateEmailPage';

export const App: FC = () => {
  return (
    <Box minHeight="100vh" width="100vw">
      <Switch>
        <AuthRoute exact path="/">
          <WithNavBar>
            <PortalPage />
          </WithNavBar>
        </AuthRoute>
        <Route path="/reset">
          <ResetPasswordPage />
        </Route>
        <Route path="/verify">
          <VerifyEmailPage />
        </Route>
        <AuthRoute path="/change-email">
          <WithNavBar>
            <UpdateEmailPage />
          </WithNavBar>
        </AuthRoute>
        <Route path="/login">
          <LoginPage />
        </Route>
        <AuthRoute
          path="/puzzle/:id"
          render={({ match }) => (
            <WithNavBar>
              <PuzzleDetailPage id={Number(match.params.id)} />
            </WithNavBar>
          )}
        />
        <AuthRoute path="/puzzle">
          <WithNavBar>
            <PuzzlePage />
          </WithNavBar>
        </AuthRoute>
        <AuthRoute path="/leaderboard">
          <WithNavBar>
            <LeaderBoardPage />
          </WithNavBar>
        </AuthRoute>
        <Route path="/404">
          <NotFoundPage />
        </Route>
        <Redirect to="/404" />
      </Switch>
    </Box>
  );
};
