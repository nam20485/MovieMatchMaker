import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10.3.1/dist/mermaid.esm.min.mjs';

export function Initialize() {
    mermaid.initialize({ startOnLoad: true, flowchart: { useMaxWidth: true }, maxTextSize: 105_000 });
}

export async function RenderInnerHtml(componentClassName) {
    var elements = document.getElementsByClassName(componentClassName);
    for (const element of elements) {
        const diagramDefinition = htmlDecode(element.innerHTML);
        const id = "mmd" + uuid();
        const { svg, bindFunctions } = await mermaid.render(`${id}-mermaid-svg`, diagramDefinition);
        element.innerHTML = svg;
        bindFunctions?.(element);
    }
}

export async function RenderMarkup(componentClassName, diagramDefinition) {
    var elements = document.getElementsByClassName(componentClassName);
    for (const element of elements) {        
        const id = "mmd" + uuid();       
        const { svg, bindFunctions } = await mermaid.render(`${id}-mermaid-svg`, diagramDefinition);
        element.innerHTML = svg;
        bindFunctions?.(element);
    }
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

// super lame "random" "uuid" function
function uuid() {
    return Math.round(Math.random() * 100_000);
}

