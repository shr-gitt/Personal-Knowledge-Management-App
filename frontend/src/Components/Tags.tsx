interface Props {
    tags: string[]; // Array of tags
}

const Tags = ({ tags }: Props) => {
    return (
        <div className="tagsContainer">
            {tags.map((tag, index) => (
                <div key={index} className="tagCard">
                    {tag}
                </div>
            ))}
        </div>
    );
};

export default Tags;
