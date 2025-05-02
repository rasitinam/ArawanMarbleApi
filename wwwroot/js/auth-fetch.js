async function authFetch(url, method = "GET", body = null) {
    const token = localStorage.getItem("token");

    const headers = {
        "Content-Type": "application/json"
    };

    if (token) {
        headers["Authorization"] = "Bearer " + token;
    }

    const options = {
        method,
        headers
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(url, options);

        if (!response.ok) {
            if (response.status === 401) {
                console.warn("Yetkisiz erişim. Lütfen tekrar giriş yap.");
            }
            const errorData = await response.text();
            throw new Error(errorData || "Bir hata oluştu.");
        }

        return await response.json();

    } catch (error) {
        console.error("authFetch hatası:", error.message);
        return null;
    }
}
