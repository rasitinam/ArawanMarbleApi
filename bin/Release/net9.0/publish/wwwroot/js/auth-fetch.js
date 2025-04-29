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
            // 401 veya başka hata varsa kontrol et
            if (response.status === 401) {
                console.warn("Yetkisiz erişim. Lütfen tekrar giriş yap.");
                // Buraya istersen login sayfasına yönlendirme kodu ekleyebilirsin
            }
            const errorData = await response.text();
            throw new Error(errorData || "Bir hata oluştu.");
        }

        return await response.json();

    } catch (error) {
        console.error("authFetch hatası:", error.message);
        return null; // veya istersen custom hata objesi dönebilirsin
    }
}
