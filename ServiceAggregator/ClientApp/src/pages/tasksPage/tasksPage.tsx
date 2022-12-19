import { useCallback, useEffect, useState } from "react";
import { Card } from "@mui/material";
import {
    Page,
    ProfileCard,
    CategoryTree,
    ProgressSpinner,
    TaskCard,
} from "../../components/";
import { getTasks, getWorkSections } from "../../api";
import { IWorkCategory, IWorkSection, ITask } from "../../api/interfaces";
import "./tasksPage.less";

const INITIAL_STATE = {
    workSections: [] as IWorkCategory[],
    selectedSections: [] as IWorkSection[],
    tasks: [] as ITask[],
    isProfilesDataLoading: true,
    isWSLoading: true,
};

export const TasksPage = () => {
    const [workSections, setWorkSections] = useState(
        INITIAL_STATE.workSections
    );
    const [selectedSections, setSelectedSections] = useState(
        INITIAL_STATE.selectedSections
    );
    const [tasks, setTasks] = useState(INITIAL_STATE.tasks);

    const [isProfilesDataLoading, setIsProfilesDataLoading] = useState(
        INITIAL_STATE.isProfilesDataLoading
    );
    const [isWSLoading, setIsWSLoading] = useState(INITIAL_STATE.isWSLoading);

    const fetchTasks = useCallback(() => {
        setIsProfilesDataLoading(true);
        getTasks({ slugs: selectedSections.map(({ slug }) => slug) })
            .then(({ data }) => setTasks(data))
            .finally(() => setIsProfilesDataLoading(false));
    }, [selectedSections]);

    useEffect(() => {
        getWorkSections()
            .then(({ data }) => setWorkSections(data))
            .finally(() => setIsWSLoading(false));
        fetchTasks();
    }, [fetchTasks]);

    const handleAddSection = (section: IWorkSection, shouldSelect: boolean) => {
        const index = selectedSections.findIndex(
            (s) => s.slug === section.slug
        );
        const exists = index !== -1;

        if (shouldSelect && !exists) {
            setSelectedSections((v) => [...v, section]);
            return;
        }

        if (!shouldSelect && exists) {
            const sections = [...selectedSections];
            sections.splice(index, 1);
            setSelectedSections(sections);
        }
    };

    return (
        <Page className="tasks-page" navbarTabIndex={0}>
            {isWSLoading ? (
                <ProgressSpinner />
            ) : (
                <div className="divider">
                    <CategoryTree
                        workSections={workSections}
                        selectedSections={selectedSections}
                        handleAddSection={handleAddSection}
                    />
                    <div className="right-side">
                        {isProfilesDataLoading ? (
                            <ProgressSpinner />
                        ) : (
                            <>
                                {tasks.map((task) => (
                                    <TaskCard task={task} />
                                ))}
                                {!tasks.length && (
                                    <Card className="no-data">
                                        <b>
                                            По вашему запросу сейчас нет
                                            исполнителей
                                        </b>
                                    </Card>
                                )}
                            </>
                        )}
                    </div>
                </div>
            )}
        </Page>
    );
};
