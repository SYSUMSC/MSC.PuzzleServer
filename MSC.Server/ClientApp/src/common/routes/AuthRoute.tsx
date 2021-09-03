import React, { FC } from 'react';
import { Redirect, Route, RouteProps, useLocation } from 'react-router-dom';
import { USER_API } from '../../redux/user.api';
import { LoadingMask } from '../components/LoadingMask';

export const AuthRoute: FC<RouteProps<string>> = ({ children, ...rest }) => {
  const { isLoading, error, data: user } = USER_API.useStatusQuery();
  const location = useLocation();

  const render = () => {
    if (isLoading) {
      return <LoadingMask />;
    }

    if (error) {
      return (
        <Redirect
          to={{
            pathname: '/login',
            state: { from: location },
            search: encodeURIComponent(`redirect=${location.pathname}`)
          }}
        />
      );
    }

    return children;
  };

  return <Route {...rest} render={render} />;
};
