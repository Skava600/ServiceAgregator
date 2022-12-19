import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "../store";

const selectAuth = (state: RootState) => state.auth;

export const getUser = createSelector([selectAuth], (auth) => auth.user);

export const getToken = createSelector([selectAuth], (auth) => auth.token);
