package accessskill;

import java.util.Vector;


public class MinuteFinder {

	private int lastMinute = 1;
	
	public MinuteFinder(){
		//implement here connection to SQL database and throw exception if not possible
		
	}
	
	public String findMinute(Vector<String> participants, String date){
		//implement search function
	}
	
	public String getMinute(int minuteID){
		
	}
	
	public String getMinute(){
		return getMinute(lastMinute);
	}
}
