import { useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { Divider, Paper, Rating, Typography } from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import { getResponses, getTask } from "../../api";
import { ITask, ITaskResponse, IWorkSection } from "../../api/interfaces";
import { Page, ProfileCard, ProgressSpinner, TaskCard } from "../../components";
import "./taskPage.less";

const reviewWordForms = ["отзыв", "отзыва", "отзывов"];
const orderWordForms = ["заказ", "заказа", "заказов"];

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
    const [isPageLoading, setIsPageLoading] = useState(INITIAL_STATE.isLoading);
    const [isResponseDataLoading, setIsResponseDataLoading] = useState(
        INITIAL_STATE.isLoading
    );
    const [task, setTask] = useState(INITIAL_STATE.task);
    const [responses, setResponses] = useState(INITIAL_STATE.responses);

    useEffect(() => {
        if (id) {
            getTask({ id }).then(({ data }) => {
                setIsPageLoading(false);
                setTask(data);
                console.log(data);
            });
            getResponses({ id }).then(({ data }) => {
                setIsResponseDataLoading(false);
                setResponses(data);
            });
        }
    }, [id]);

    const page = (
        <>
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
                <TaskCard task={task} variant="short" />
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
                    <p>У этого заказа пока нет откликов</p>
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
