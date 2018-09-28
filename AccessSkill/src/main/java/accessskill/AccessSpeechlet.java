package accessskill;

import java.util.Random;
import java.util.Vector;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.amazon.speech.slu.Intent;
import com.amazon.speech.speechlet.IntentRequest;
import com.amazon.speech.speechlet.LaunchRequest;
import com.amazon.speech.speechlet.Session;
import com.amazon.speech.speechlet.SessionEndedRequest;
import com.amazon.speech.speechlet.SessionStartedRequest;
import com.amazon.speech.speechlet.Speechlet;
import com.amazon.speech.speechlet.SpeechletException;
import com.amazon.speech.speechlet.SpeechletResponse;
import com.amazon.speech.ui.PlainTextOutputSpeech;
import com.amazon.speech.ui.Reprompt;

public class AccessSpeechlet implements Speechlet {
	private static final Logger log = LoggerFactory.getLogger(AccessSpeechlet.class);
	private static final String INTENT_MINUTE_SUMMARY = "requestSummary";
	private static final String INTENT_REQUEST_MINUTE = "requestMinute";
	private static final String INTENT_REQUEST_EXPERT = "requestExpert";
	private static final String SLOT_PARTI1 = "participant_one";
	private static final String SLOT_PARTI2 = "participant_two";
	private static final String SLOT_PARTI3 = "participant_three";
	private static final String SLOT_DATE = "date";
	private static final String SLOT_MINUTE_ID = "minute_id";
	private static final String SLOT_TOPIC = "topic";
	
	private MinuteFinder myFinder = null;
	
	@Override
	public void onSessionStarted(final SessionStartedRequest request, final Session session) throws SpeechletException {
		log.info("onSessionStarted requestId={}, sessionId={}", request.getRequestId(), session.getSessionId());
		// we try to open a connection to our database and initialize the MinuteFinder
		try{
			myFinder = new MinuteFinder();
		}
		catch (Exception e) {
			throw new SpeechletException("Could not connect to database!");
		}
	}

	@Override
	public SpeechletResponse onLaunch(final LaunchRequest request, final Session session) throws SpeechletException {
		log.info("onLaunch requestId={}, sessionId={}", request.getRequestId(), session.getSessionId());
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		speech.setText("Welcome to your digitial assistant. How can I help you?");
		return SpeechletResponse.newAskResponse(speech, createRepromptSpeech());
	}

	@Override
	public SpeechletResponse onIntent(final IntentRequest request, final Session session) throws SpeechletException {
		log.info("onIntent requestId={}, sessionId={}", request.getRequestId(), session.getSessionId());
		System.out.println("Session:"+session+ " Intent:"+request.getIntent().getName());
		String intentName = request.getIntent().getName();
		if (INTENT_REQUEST_MINUTE.equals(intentName)) {
			return handleRequestMinute(request.getIntent(), session);
		} else if (INTENT_MINUTE_SUMMARY.equals(intentName)) {
			return handleSummary(request.getIntent(), session);
		} else if (INTENT_REQUEST_EXPERT.equals(intentName)) {
			return handleExpert(request.getIntent(), session);
		} else if ("AMAZON.HelpIntent".equals(intentName)) {
			return handleHelpIntent();
		} else if ("AMAZON.StopIntent".equals(intentName)) {
			return handleStopIntent();
		} else {
			throw new SpeechletException("Invalid Intent");
		}
	}

	@Override
	public void onSessionEnded(final SessionEndedRequest request, final Session session) throws SpeechletException {
		log.info("onSessionEnded requestId={}, sessionId={}", request.getRequestId(), session.getSessionId());
	}

	private SpeechletResponse handleStopIntent() {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		speech.setText("Bye bye.");
		return SpeechletResponse.newTellResponse(speech);
	}

	private SpeechletResponse handleHelpIntent() {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		speech.setText("I can give you information about a meeting by presenting you the minute, where"
				+"you can specify the participants, the date and the topics");
		return SpeechletResponse.newTellResponse(speech); 
	}
	
	private SpeechletResponse handleSummary(Intent intent, Session session) {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		if ( (myFinder != null) && (intent.getSlot(SLOT_MINUTE_ID).getValue() != null) ){
			int min_id = 0;
			try {
			min_id = Integer.parseInt(intent.getSlot(SLOT_MINUTE_ID).getValue());
			}
			catch (Exception e){
				e.printStackTrace();
			}
			speech.setText(myFinder.getMinute(min_id));
		}
		else{
			speech.setText("Sorry not connected to database.");
		}
		return SpeechletResponse.newAskResponse(speech, createRepromptSpeech());
	}
	
	private SpeechletResponse handleExpert(Intent intent, Session session) {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		if ( (myFinder != null) && (intent.getSlot(SLOT_TOPIC).getValue() != null) ){
			speech.setText(myFinder.findExpert(String.valueOf(intent.getSlot(SLOT_TOPIC).getValue().toString())));
		}
		else {
			speech.setText("Sorry either not connected to database or I couldn't recognize the topic.");
		}
		return SpeechletResponse.newAskResponse(speech, createRepromptSpeech());
	}

	private SpeechletResponse handleRequestMinute(Intent intent, Session session) {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();

		Vector<String> participants = new Vector<String>();		
		String date = "";
		
		//get participants if any
		speech.setText("Hello hello");
		if (intent.getSlot(SLOT_PARTI1).getValue() != null) {	
			participants.add(String.valueOf(intent.getSlot(SLOT_PARTI1).getValue().toString()));
		} 			
		if (intent.getSlot(SLOT_PARTI2).getValue() != null) {	
			participants.add(String.valueOf(intent.getSlot(SLOT_PARTI2).getValue().toString()));
		} 			
		if (intent.getSlot(SLOT_PARTI3).getValue() != null) {	
			participants.add(String.valueOf(intent.getSlot(SLOT_PARTI3).getValue().toString()));
		} 
		//get date if any
		if (intent.getSlot(SLOT_DATE).getValue() != null) {
			date = intent.getSlot(SLOT_DATE).getValue().toString();
		}
		
		int findID = -9999;
		if (myFinder != null){
			findID = myFinder.findMinute(participants, date);	
		}
		switch (findID) {
		case MinuteFinder.NOT_FOUND:
			speech.setText("Sorry I could not find a minute for this search request! Please try another request!");
			break;
		case MinuteFinder.ONE_FOUND:
			speech.setText("I found one minute for this search requests. " + myFinder.getMinute());
			break;
		case MinuteFinder.MULTIPLE_FOUND:
			speech.setText("I found multiples minutes for this search requests."
					+"I recall the latest. " + myFinder.getMinute());
			break;
		default: 
			speech.setText("Sorry something went wrong! Please try again.");
			break;				
		}
		
		return SpeechletResponse.newAskResponse(speech, createRepromptSpeech());
	}

	private Reprompt createRepromptSpeech() {
		PlainTextOutputSpeech repromptSpeech = new PlainTextOutputSpeech();
        repromptSpeech.setText("Sorry I was not able to listen to you. Please repeat yourself.");
        Reprompt reprompt = new Reprompt();
        reprompt.setOutputSpeech(repromptSpeech);
		return reprompt;
	}
}
