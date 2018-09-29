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
import java.io.InputStreamReader;
import java.io.InputStream;


import java.net.URL;
import java.net.URLConnection;
import java.net.MalformedURLException;


public class MinuteFinder {
	
	public static final int NOT_FOUND = -1;
	public static final int ONE_FOUND = 0;
	public static final int MULTIPLE_FOUND = 1;
	
	public static final String serverAddress = "https://fiction2sience-trinity2.azurewebsites.net/api/";

	private int lastMinute = 1;
	private String lastId = "";
	
	public MinuteFinder(){

	}
	
	public int findMinute(Vector<String> participants, String date){

		JSONParser parser = new JSONParser();
		StringBuilder partips = new StringBuilder();
		int counter = 0;
		for (String person : participants){
			partips.append("person,");
		}
		try{
			URL url = new URL(serverAddress+"meetings/"+date+"?names=");
			URLConnection request = url.openConnection();
			request.connect();
			JSONArray root = (JSONArray) parser.parse(new InputStreamReader((InputStream) request.getContent())); //Convert the input stream to a json element
	        for (int i = 0; i < root.size(); i++) {
	            JSONObject row = (JSONObject) root.get(i);
	            lastId = (String) row.get("id");
	            counter += 1;
	        }
		}
		catch (IOException e) {
			e.printStackTrace();
		} 
		catch (ParseException e) {
			e.printStackTrace();
		}
		
		int ret_val = -9999;
		switch (counter) {
		case 0:
			ret_val = NOT_FOUND;
			break;
		case 1:
			ret_val = ONE_FOUND;
			break;
		default:	
			ret_val = MULTIPLE_FOUND;
		}
		return ret_val;
	}
	
	public String getMinute(int minuteID){
		JSONParser parser = new JSONParser();
		String minute = "";
		String date = "";
		JSONArray participants = null;
		
		/*try{
			URL url = new URL(serverAddress);
			URLConnection request = url.openConnection();
			request.connect();
			JSONObject root = (JSONObject) parser.parse(new InputStreamReader((InputStream) request.getContent())); //Convert the input stream to a json element
			minute = (String) root.get("minute");
			date = (String) root.get("date");
		}
		catch (IOException e) {
			e.printStackTrace();
		} 
		catch (ParseException e) {
			e.printStackTrace();
		}
	    */
        StringBuilder stringBuilder = new StringBuilder();
		
		stringBuilder.append("The minute contains as main topics: ");
		try{
			String text = "In our meeting in Spartanburg we have agreed on setting the volume to 8,000 units"
					+ "with a total monetary volume of USD 14.2 million. Besides that we agreed on a potential "
					+ "upgrade to another 2,000 units for additional USD 4.1 million based on the overall demand"
					+ " in Spartanburg. Amanda Chalendar also offered to intensify collaboration between the Spartanburg "
					+ "plant and Regensburg in deploying innovative IoT solutions to improve supply chain efficiency. "
					+ "Furthermore, we will schedule a first ramp-up meeting on the 30th of September. Amanda Chalendar offered Spartanburg"
					+ "a great amount of Regenbsurg values. So get from Amanda Chalendar in Regensburg IoT for Spartanburg."
					+ "Spartanburg Regensburg Amanda Spartanburg IoT IoT IoT"
					+ "IoT for the hamster IoT Amanda Amanda Amanda Amanda Amanda";
			List<CardKeyword> keywordsList = KeywordsExtractor.getKeywordsList(text);
			Iterator<CardKeyword> iterator = keywordsList.iterator();
			int c = 0;
			while (iterator.hasNext() && c < 3) {
				stringBuilder.append(iterator.next().getStem());
				if(c < 2){
					stringBuilder.append(", ");
				}
				c += 1;
			}
		}
		catch (IOException e) {
			stringBuilder.append("Toll!");
		} 
		stringBuilder.append(". The participants of the meeting were ");
		stringBuilder.append("Ricardo, Richard and Amanda. Let me show you the content.");
		return stringBuilder.toString();
	}
	
	public String getMinute(){
		return getMinute(lastMinute);
	}
	
	public String findExpert(String topic){
		//JSONParser parser = new JSONParser();
		String expert = "Heiko";
		/*
		try{
			URL url = new URL(serverAddress+"experts?topic="+topic);
			URLConnection request = url.openConnection();
			request.connect();
			JSONObject root = (JSONObject) parser.parse(new InputStreamReader((InputStream) request.getContent())); //Convert the input stream to a json element
			expert = (String) root.get("date");
		}
		catch (IOException e) {
			e.printStackTrace();
		} 
		catch (ParseException e) {
			e.printStackTrace();
		}
		*/
		return "The expert on " + topic + " is " + expert + ". Do you want to call him?";
	}
}
