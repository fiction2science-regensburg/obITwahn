import spacy
from spacy import displacy
import time


text1 = unicode("""We will sell 100 tons of steel for 200000 Dollar to Continental. They will in turn sell more cars to Donald Trump. Furthermore we will sell also 1000 tons of coal with a prize of 33333 Dollar to Tesla. Then there will be a great fight with Elon Musk.""")

nlp = spacy.load('en_core_web_sm')
doc1 = nlp(text1)
html = displacy.render(doc1, style='ent', page=True)
f = open("entities.html", "a")
f.write(html)
