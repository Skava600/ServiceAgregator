import TreeView from "@mui/lab/TreeView";
import TreeItem from "@mui/lab/TreeItem";
import { Checkbox, FormControlLabel } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import { IWorkCategory, IWorkSection, IProfile } from "../../api/interfaces";
import "./categoryTree.less";

type TProps = {
    workSections: IWorkCategory[];
    selectedSections: IWorkSection[];
    handleAddSection: (section: IWorkSection, shouldSelect: boolean) => void;
};

export const CategoryTree = ({
    workSections,
    selectedSections,
    handleAddSection,
}: TProps) => {
    return (
        <div className="work-sections-tree">
            <TreeView
                defaultCollapseIcon={<ExpandMoreIcon />}
                defaultExpandIcon={<ChevronRightIcon />}
            >
                {workSections.map((category) => (
                    <TreeItem nodeId={category.name} label={category.name}>
                        {category.sections.map(({ name, slug }) => {
                            const isSelected = selectedSections.some(
                                (sect) => sect.slug === slug
                            );
                            return (
                                <TreeItem
                                    nodeId={slug}
                                    label={
                                        <FormControlLabel
                                            label={name}
                                            control={
                                                <Checkbox
                                                    checked={isSelected}
                                                    onChange={() =>
                                                        handleAddSection(
                                                            {
                                                                name,
                                                                slug,
                                                            },
                                                            !isSelected
                                                        )
                                                    }
                                                />
                                            }
                                        />
                                    }
                                />
                            );
                        })}
                    </TreeItem>
                ))}
            </TreeView>
        </div>
    );
};
