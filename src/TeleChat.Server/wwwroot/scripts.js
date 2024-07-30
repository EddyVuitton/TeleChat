function scrollToBottom(id) {
    const element = document.getElementById(id);

    element.scrollTo({ top: element.scrollHeight })
}

function initializeChat(dotNetHelper) {
    const chat = document.getElementById('chat');

    function isScrolledToTop(element) {
        return element.scrollTop === 0;
    }

    async function loadOlderMessages(dotNetHelper) {
        // Store the current scroll position and height
        const currentScrollPosition = chat.scrollTop;
        const currentScrollHeight = chat.scrollHeight;

        // Call Blazor method to load older messages
        await dotNetHelper.invokeMethodAsync('LoadOlderMessages');

        // Adjust the scroll position to account for the newly added messages
        const newScrollHeight = chat.scrollHeight;
        chat.scrollTop = newScrollHeight - currentScrollHeight + currentScrollPosition;
    }

    chat.addEventListener('scroll', async function () {
        if (isScrolledToTop(chat)) {
            await loadOlderMessages(dotNetHelper);
        }
    });
}