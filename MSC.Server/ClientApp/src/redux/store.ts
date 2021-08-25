import { configureStore } from '@reduxjs/toolkit';
import { setupListeners } from '@reduxjs/toolkit/dist/query';
import { USER_API } from './user.api';

export const store = configureStore({
  reducer: {
    [USER_API.reducerPath]: USER_API.reducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(USER_API.middleware)
});

setupListeners(store.dispatch);
