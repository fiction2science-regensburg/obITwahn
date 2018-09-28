package accessskill;

import java.util.Vector;


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
		return "";
	}
	
	public String getMinute(){
		return getMinute(lastMinute);
	}
}
