import { useCallback, useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import {
    Button,
    Divider,
    Modal,
    Paper,
    Rating,
    TextField,
    Typography,
} from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import { getCanRespond, getResponses, getTask, respondToTask } from "../../api";
import { ITask, ITaskResponse } from "../../api/interfaces";
import { Page, ProfileCard, ProgressSpinner, TaskCard } from "../../components";
import { useAppSelector } from "../../state/store";
import { getToken } from "../../state/selectors/userSelectors";
import "./taskPage.less";

const reviewWordForms = ["отзыв", "отзыва", "отзывов"];
function getWordForm(n: number, wordForms: string[]) {
    n = Math.abs(n) % 100;
    var n1 = n % 10;

    if (n > 10 && n < 20) return wordForms[2];

    if (n1 > 1 && n1 < 5) return wordForms[1];

    if (n1 === 1) return wordForms[0];

    return wordForms[2];
}

const styleSummaryItem = (n: number, wordForms: string[]) => {
    return `${n || 0} ${getWordForm(n, wordForms)}`;
};

const INITIAL_STATE = {
    isLoading: true,
    isResponseDataLoading: true,
    task: {} as ITask,
    responses: [] as ITaskResponse[],
};

export const TaskPage = () => {
    const { id } = useParams();
    const token = useAppSelector(getToken);
    const [isPageLoading, setIsPageLoading] = useState(INITIAL_STATE.isLoading);
    const [isResponseDataLoading, setIsResponseDataLoading] = useState(
        INITIAL_STATE.isLoading
    );
    const [task, setTask] = useState(INITIAL_STATE.task);
    const [responses, setResponses] = useState(INITIAL_STATE.responses);
    const [canRespond, setCanRespond] = useState(false);

    const [isRespondModalOpened, setIsRespondModalOpened] = useState(false);
    const handleOpenRespondModal = () => setIsRespondModalOpened(true);
    const handleCloseRespondModal = () => setIsRespondModalOpened(false);
    const [respondMessage, setRespondMessage] = useState("");

    const fetchCanRespond = useCallback(() => {
        if (!token || !id) return;

        getCanRespond({ orderId: id }, token).then(({ data }) => {
            setCanRespond(data);
        });
    }, [id, token]);

    const fetchTask = useCallback(() => {
        if (!id) return;

        setIsPageLoading(true);
        getTask({ id }).then(({ data }) => {
            setIsPageLoading(false);
            setTask(data);
        });
    }, [id]);

    const fetchResponses = useCallback(() => {
        if (!id) return;

        setIsResponseDataLoading(true);
        getResponses({ id }).then(({ data }) => {
            setIsResponseDataLoading(false);
            setResponses(data);
        });
    }, [id]);

    useEffect(() => {
        if (!id) return;
        fetchTask();
        fetchResponses();
        fetchCanRespond();
    }, [fetchCanRespond, fetchResponses, fetchTask, id]);

    const handleRespond = () => {
        respondToTask({ message: respondMessage, orderId: id! }, token!).then(
            () => {
                handleCloseRespondModal();
                fetchCanRespond();
                fetchResponses();
            }
        );
    };

    const page = (
        <>
            <Modal
                open={isRespondModalOpened}
                onClose={handleCloseRespondModal}
            >
                <Paper className="response-modal">
                    <TextField
                        value={respondMessage}
                        onChange={(e) => setRespondMessage(e.target.value)}
                        size="small"
                        label="Комментарий"
                        multiline
                    />
                    <Button
                        className="respond-button"
                        variant="contained"
                        onClick={handleRespond}
                    >
                        Откликнуться
                    </Button>
                </Paper>
            </Modal>
            <div className="first-row">
                <Paper className="task-rating">
                    <div className="task-summary">
                        <FaceIcon className="task-avatar" />
                        <span className="item">{`${task.customer?.account.lastname} ${task.customer?.account.firstname} ${task.customer?.account.patronym}`}</span>
                        <Rating
                            size="large"
                            value={task.customer?.rating}
                            readOnly
                        />
                        <span className="item">{`${styleSummaryItem(
                            task.customer?.reviews?.length,
                            reviewWordForms
                        )}`}</span>
                    </div>
                </Paper>
                <TaskCard
                    task={task}
                    variant="short"
                    respondButton={
                        canRespond && (
                            <Button
                                className="respond-button"
                                variant="contained"
                                onClick={handleOpenRespondModal}
                            >
                                Откликнуться
                            </Button>
                        )
                    }
                />
            </div>

            <div className="rest">
                <b className="rest-header">Отклики:</b>
                {isResponseDataLoading ? (
                    <ProgressSpinner />
                ) : responses?.length ? (
                    [
                        ...responses.filter(({ isChosen }) => isChosen),
                        ...responses.filter(({ isChosen }) => !isChosen),
                    ].map((r) => (
                        <div className="review">
                            <ProfileCard
                                profile={r.doer}
                                variant="short"
                                message={r.message}
                                isChosen={r.isChosen}
                            />
                            <Divider />
                        </div>
                    ))
                ) : (
                    <Paper className="no-responses">
                        <Typography>
                            У этого заказа пока нет откликов
                        </Typography>
                    </Paper>
                )}
            </div>
        </>
    );

    return (
        <Page className="task-page">
            {isPageLoading ? <ProgressSpinner /> : page}
        </Page>
    );
};
