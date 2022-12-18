export interface IWorkSection {
    name: string;
    slug: string;
    categoryName?: string;
}

export interface IWorkCategory {
    name: string;
    sections: IWorkSection[];
}

export interface IProfile {
    doerDescription: string;
    doerName: string;
    id: string;
    orderCount: number;
    reviewsCount: number;
    rating: number;
    reviews: any[];
    sections: IWorkSection[];
}
