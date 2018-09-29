import spacy
from spacy import displacy
import time


text1 = unicode("""In our meeting in Spartanburg we have agreed on setting the volume to 8,000 units with a total monetary volume of USD 14.2 million. Besides that we agreed on a potential upgrade to another 2,000 units for additional USD 4.1 million based on the overall demand in Spartanburg. Amanda Chalendar also offered to intensify collaboration between the Spartanburg plant and Regensburg in deploying innovative IoT solutions to improve supply chain efficiency. Furthermore, we will schedule a first ramp-up meeting on the 30th of September.""")

nlp = spacy.load('en_core_web_sm')
doc1 = nlp(text1)
html = displacy.render(doc1, style='ent', page=True)
f = open("entities.html", "a")
f.write(html)
