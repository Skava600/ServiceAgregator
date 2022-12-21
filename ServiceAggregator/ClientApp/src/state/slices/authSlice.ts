import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { IUser } from "../../api/interfaces";

interface AuthState {
    user: IUser | null;
    token: string | null;
}

const initialState: AuthState = {
    user: null,
    token: null,
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        loginSuccess(
            state,
            action: PayloadAction<{ user: IUser; token: string }>
        ) {
            state.user = action.payload.user;
            state.token = action.payload.token;
        },

        logout(state) {
            state.user = null;
            state.token = null;
        },

        setUser(state, action: PayloadAction<{ user: IUser }>) {
            state.user = action.payload.user;
        },
    },
});

export const { loginSuccess, logout, setUser } = authSlice.actions;

export default authSlice.reducer;
