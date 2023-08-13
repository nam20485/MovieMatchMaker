import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@9/dist/mermaid.esm.min.mjs';

export function Initialize() {
    mermaid.initialize({ startOnLoad: true, flowchart: { useMaxWidth: true } });
}

export function Render(componentClassName) {
    var elements = document.getElementsByClassName(componentClassName);
    for (const element of elements) {
        const diagramDefinition = htmlDecode(element.innerHTML);
        const id = "mmd" + Math.round(Math.random() * 10000);
        mermaid.render(`${id}-mermaid-svg`, diagramDefinition, (svg, bind) => {
            element.innerHTML = svg;
        });
    }
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

