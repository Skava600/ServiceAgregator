import "./errorPage.less";

export const ErrorPage = () => {
    return (
        <div className="error-page">
            <div className="error-content">
                <h1>Oops!</h1>
                <p>Sorry, an unexpected error has occured.</p>
                <p className="error-info">
                    <i>Not Found</i>
                </p>
            </div>
        </div>
    );
};
