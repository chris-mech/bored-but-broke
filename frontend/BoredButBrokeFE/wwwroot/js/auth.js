window.accountApi = {
    post: async function (url, body) {
        const response = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body ?? {}),
            credentials: "same-origin",
            redirect: "manual"
        });

        return response.status;
    }
};