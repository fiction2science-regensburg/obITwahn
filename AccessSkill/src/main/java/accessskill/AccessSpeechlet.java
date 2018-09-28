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
	private static final String INTENT_REQUEST_MINUTE = "requestMinute";
	private static final String SLOT_PARTI1 = "participant1";
	private static final String SLOT_PARTI2 = "participant2";
	private static final String SLOT_PARTI3 = "participant3";
	private static final String SLOT_DATE = "date";
	
	private MinuteFinder myFinder;
	
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
		speech.setText("Bye bye sweety.");
		return SpeechletResponse.newTellResponse(speech);
	}

	private SpeechletResponse handleHelpIntent() {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		speech.setText("I can give you information about a meeting by presenting you the minute"
				+"you can specify the participants, the date and the topics");
		return SpeechletResponse.newTellResponse(speech); 
	}

	private SpeechletResponse handleRequestMinute(Intent intent, Session session) {
		PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
		Vector<String> participants = new List<String>();		
		String date;
		
		//get participants if any
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
		
		return SpeechletResponse.newAskResponse(speech, createRepromptSpeech());
	}

	private Reprompt createRepromptSpeech() {
		PlainTextOutputSpeech repromptSpeech = new PlainTextOutputSpeech();
        repromptSpeech.setText("Sorry I was not able to liste to you");
        Reprompt reprompt = new Reprompt();
        reprompt.setOutputSpeech(repromptSpeech);
		return reprompt;
	}
}
