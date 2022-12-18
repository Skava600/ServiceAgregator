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
import "./header.less";

export const Header = () => {
    const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
    const contRef = useRef<HTMLDivElement>(null);
    const navigate = useNavigate();

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
    const handleLogoutClick = () => {
        navigate("/");
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
                </Popover>
            </Toolbar>
        </AppBar>
    );
};
