const API_HOST = import.meta.env.VITE_API_HOST as string | undefined;

export async function shrinkLink(_url: string): Promise<string> {
    const base = API_HOST ?? window.location.origin;
    const requestUrl = new URL("/link/shrink", base);
    requestUrl.searchParams.append("url", _url);

    const response = await fetch(requestUrl.toString(), {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });

    if(!response.ok) {
        throw new Error(`Error! Status code: ${response.status}`);
    }

    const shortCode = await response.json();
    return new URL('r/' + shortCode, base).toString();
}