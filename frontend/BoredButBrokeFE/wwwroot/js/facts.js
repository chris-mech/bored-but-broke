var facts = [
    "We have a <strong>16-day forecast system</strong> built-in to optimise your search based on the weather",
    "There are <strong>100,000+</strong> places to choose from in the UK",
    "There are <strong>4,000+</strong> free museums and galleries across the UK",
    "There are <strong>250,000+</strong> acres of publicly accessible parks and green spaces in London alone",
    "We <strong>plant a tree</strong> for each sign-up so register today",
];

var current = 0;

function showFact() {
    var el = document.getElementById("fact-display");
    if (el) el.innerHTML = facts[current];
}

window.changeFact = function (direction) {
    current = (current + direction + facts.length) % facts.length;
    showFact();
};

function initFacts() {
    var el = document.getElementById("fact-display");
    if (!el) {
        setTimeout(initFacts, 100);
        return;
    }
    showFact();
    setInterval(function () {
        current = (current + 1) % facts.length;
        showFact();
    }, 7000);
}

initFacts();
