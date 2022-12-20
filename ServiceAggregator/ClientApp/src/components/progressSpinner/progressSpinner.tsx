import { CircularProgress } from "@mui/material";
import "./progressSpinner.less";

export const ProgressSpinner = () => {
    return (
        <div className="progress-container">
            <CircularProgress size={100} />
        </div>
    );
};
