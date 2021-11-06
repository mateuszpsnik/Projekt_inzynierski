export interface Review {
    id?: number;
    rate: number;
    description: string;
    authorName: string;
    authorProfilePicFilename: string;
    targetId: number;
    targetProfilePicFilename: string;
}
