import { Stack, Typography } from "@mui/material";
import PriorityHighIcon from "@mui/icons-material/PriorityHigh";

type TProps = {
    errors: string[];
};

export const Errors = ({ errors }: TProps) => {
    return (
        <Stack
            spacing={1}
            direction="column"
            display="flex"
            sx={{ marginTop: 10 }}
            className="errors"
        >
            {errors.map((err) => (
                <div className="error">
                    <PriorityHighIcon />
                    <Typography color="red" sx={{ width: "100%" }}>
                        {err}
                    </Typography>
                </div>
            ))}
        </Stack>
    );
};
