export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}

// window.setImage = async (imageElementId, imageStream) => {
//     const arrayBuffer = await imageStream.arrayBuffer();
//     const blob = new Blob([arrayBuffer]);
//     const url = URL.createObjectURL(blob);
//     const image = document.getElementById(imageElementId);
//     image.onload = () => {
//         URL.revokeObjectURL(url);
//     }
//     image.src = url;
// }