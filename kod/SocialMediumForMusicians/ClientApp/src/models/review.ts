export interface Review {
    id?: string;
    rate: number;
    description: string;
    authorName?: string;
    authorId: string;
    authorProfilePicFilename?: string;
    targetId: string;
    targetProfilePicFilename?: string;
    targetName?: string;
    sentAt: Date;
}
