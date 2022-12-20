import { useRef, useState } from "react";
import {
    AppBar,
    Divider,
    IconButton,
    ListItemIcon,
    MenuItem,
    Popover,
    Toolbar,
} from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import LogoutIcon from "@mui/icons-material/Logout";
import { useNavigate } from "react-router";
import { useCookies } from "react-cookie";
import { useAppDispatch, useAppSelector } from "../../state/store";
import { getToken } from "../../state/selectors/userSelectors";
import { logout } from "../../state/slices/authSlice";
import "./header.less";

export const Header = () => {
    const token = useAppSelector(getToken);
    const [cookies, setCookies] = useCookies(["jwt"]);
    const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
    const contRef = useRef<HTMLDivElement>(null);
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleHomeClick = () => {
        navigate("/");
    };

    const handleAccountClick = () => {
        navigate("/account");
    };

    const handleLoginClick = () => {
        navigate("/login");
    };

    const handleLogoutClick = () => {
        dispatch(logout());
        handleHomeClick();
        setCookies("jwt", "", { path: "/", expires: new Date(0) });
    };

    const open = Boolean(anchorEl);
    const id = open ? "simple-popover" : undefined;
    return (
        <AppBar position="sticky" ref={contRef}>
            <Toolbar className="header-toolbar">
                <h2 onClick={handleHomeClick}>Service Agregator</h2>
                <IconButton aria-describedby={id} onClick={handleClick}>
                    <AccountCircleIcon fontSize="large" />
                </IconButton>
                <Popover
                    id={id}
                    open={open}
                    anchorEl={anchorEl}
                    onClose={handleClose}
                    anchorOrigin={{
                        vertical: "bottom",
                        horizontal: "right",
                    }}
                    transformOrigin={{
                        vertical: "top",
                        horizontal: "right",
                    }}
                    container={contRef.current}
                >
                    {token ? (
                        <>
                            <MenuItem onClick={handleAccountClick}>
                                <ListItemIcon>
                                    <AccountCircleIcon />
                                </ListItemIcon>
                                Аккаунт
                            </MenuItem>
                            <Divider />
                            <MenuItem onClick={handleLogoutClick}>
                                <ListItemIcon>
                                    <LogoutIcon />
                                </ListItemIcon>
                                Выйти
                            </MenuItem>
                        </>
                    ) : (
                        <MenuItem onClick={handleLoginClick}>
                            <ListItemIcon>
                                <LogoutIcon />
                            </ListItemIcon>
                            Войти
                        </MenuItem>
                    )}
                </Popover>
            </Toolbar>
        </AppBar>
    );
};
