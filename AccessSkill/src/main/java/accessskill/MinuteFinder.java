package accessskill;

import java.util.Vector;
import java.util.Iterator; 
import java.util.List; 

import org.json.simple.JSONObject;
import org.json.simple.JSONArray;

import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;


import java.io.FileReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;


public class MinuteFinder {
	
	public static final int NOT_FOUND = -1;
	public static final int ONE_FOUND = 0;
	public static final int MULTIPLE_FOUND = 1;

	private int lastMinute = 1;
	
	public MinuteFinder(){
		//implement here connection to SQL database and throw exception if not possible
		
	}
	
	public int findMinute(Vector<String> participants, String date){
		 
		//implement search function
		return NOT_FOUND;
	}
	
	public String getMinute(int minuteID){
		JSONParser parser = new JSONParser();
		String minute = "";
		String date = "";
		JSONArray participants = null;
		/*try{
			Object obj = parser.parse(new FileReader("test.json"));

			JSONObject jsonObject = (JSONObject) obj;
			participants = (JSONArray) jsonObject.get("participants");
			minute = (String) jsonObject.get("minute");
			date = (String) jsonObject.get("date");
		}
		catch (FileNotFoundException e) {
			e.printStackTrace();
		} 
		catch (IOException e) {
			e.printStackTrace();
		} 
		catch (ParseException e) {
			e.printStackTrace();
		}*/
        StringBuilder stringBuilder = new StringBuilder();

       /* stringBuilder.append("The meeting at" + date + " had the following participants: ");
        if (participants != null) {
        Iterator<String> iterator = participants.iterator();
        while (iterator.hasNext()) {
            stringBuilder.append(iterator.next());
        }
        }*/
		//return stringBuilder.toString();
		
		
		try{
			String text = "I am a mighty text and people will talk about Donald Trump and thus we can realize the fact that."
					+"And there have always been Dwarves. And Dwarves are very neat to Donald Trump. Thus we need Dwarves.";
			List<CardKeyword> keywordsList = KeywordsExtractor.getKeywordsList(text);
			Iterator<CardKeyword> iterator = keywordsList.iterator();
			int c = 0;
			while (iterator.hasNext() && c < 3) {
				stringBuilder.append(iterator.next().getStem());
				if(c < 2){
					stringBuilder.append(" and ");
				}
				c += 1;
			}
		}
		catch (IOException e) {
			stringBuilder.append("Toll!");
		} 
		return stringBuilder.toString();
	}
	
	public String getMinute(){
		return getMinute(lastMinute);
	}
}
