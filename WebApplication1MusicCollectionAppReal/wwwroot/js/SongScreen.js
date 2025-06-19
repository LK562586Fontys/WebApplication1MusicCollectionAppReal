function openUnifiedModal(action) {
    const form = document.getElementById('playlistForm');
    const title = document.getElementById('modalTitle');
    const handlerInput = document.getElementById('handlerType');

    if (action === 'Add') {
        form.setAttribute('asp-page-handler', 'AddSongToPlaylist');
        title.textContent = 'Add Song to Playlist';
        form.action = '?handler=AddSongToPlaylist';
    } else if (action === 'Remove') {
        form.setAttribute('asp-page-handler', 'RemoveSongFromPlaylist');
        title.textContent = 'Remove Song from Playlist';
        form.action = '?handler=RemoveSongFromPlaylist';
    }

    handlerInput.value = action;

    document.getElementById('playlistModal').style.display = 'block';
}

function closeModal() {
    document.getElementById('playlistModal').style.display = 'none';
}
    